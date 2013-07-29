using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using FISCA.Synchronization;
using System;

namespace K12.Data.Utility
{
    /// <summary>
    /// Physical Table ChangeSet Provider.
    /// </summary>
    public class PTChangeSetProvider : FISCA.Synchronization.IChangeSetProvider
    {
        private const string GETLASTSEQUENCE_SERVICENAME = "DataSynchronization.GetLastSequence";
        private const string GETCHANGESET_SERVICENAME = "DataSynchronization.GetChangeSet";
        private long CurrentSequence { get; set; }
        private Dictionary<string, List<ChangeEntry>> mClientChangeSet;

        public PTChangeSetProvider()
        {
            mClientChangeSet = new Dictionary<string, List<ChangeEntry>>();
        }



        #region IChangeSetProvider 成員
        /// <summary>
        /// 取得異動集合
        /// </summary>
        /// <returns></returns>
        public List<ChangeEntry> GetChangeSet()
        {
            List<ChangeEntry> ChangeEntries = new List<ChangeEntry>();

            DSXmlHelper helper = new DSXmlHelper("Request");

            //<Request>
            //<All/>
            //<Condition>
            //<Sequence>1</Sequence>
            //</Condition>
            //</Request>
            helper.AddElement("All");
            helper.AddElement("Condition");
            helper.AddElement("Condition", "Sequence", CurrentSequence.ToString());

            DSRequest Req = new DSRequest(helper.BaseElement);

            DSXmlHelper rsp = DSAServices.CallService(GETCHANGESET_SERVICENAME, Req).GetContent();

            //從Server端取回多筆的ChangeEntry
            foreach (XmlElement element in rsp.GetElements("Change"))
            {
                //將ChangeEntry的XML格式轉為物件
                ChangeEntry Entry = new ChangeEntry();
                Entry.Load(element);

                //將ChangeEntry根據表格名稱、異動類別（Insert、Update、Delete）及鍵值組合成異動鍵值
                string UID = Entry.TableName + "-" + Entry.Action.ToString() + "-" + Entry.DataID;

                Console.WriteLine("UID:"+UID);
                Console.WriteLine("ClientChangeSetCount:"+mClientChangeSet.Count);

                //假設在ClientChangeSet當中有對應的異動鍵值，並且該異動鍵值裡的集合數大於零，那麼就進行合併消去動作
                if (mClientChangeSet.ContainsKey(UID) && mClientChangeSet[UID].Count>0)
                {
                    List<ChangeEntry> RemoveEntries = new List<ChangeEntry>();

                    foreach (ChangeEntry LocalEntry in mClientChangeSet[UID])
                    {
                        //狀況一：本機異動（ClientChangeEntry）與遠端異動（ServerChangeEntry）的異動筆數（Count）一樣：
                        //1.遠端異動（ServerChangeEntry）不加入到實際異動（GetChangeSet的傳回值）。
                        //2.本機異動（ClientChangeEntry）移除。
                        //3.發生情況：當新增本機異動至取得遠端異動期間，只有本機對於該筆資料做異動。
                        if (LocalEntry.Count == Entry.Count)
                        {
                            Console.WriteLine("狀況一");
                            RemoveEntries.Add(LocalEntry); //將ClientChangeEntry及ServerChangeEntry進行對消。
                        }
                        //狀況二：本機異動（ClientChangeEntry）大於遠端異動（ServerChangeEntry）的異動筆數（Count）：
                        //1.遠端異動（ServerChangeEntry）不加入到實際異動（GetChangeSet的傳回值）。
                        //2.本機異動（ClientChangeEntry）的異動筆數減去遠端異動（ServerChangeEntry）並保留本機異動。
                        //3.發生情況：理論上不會有此種情況發生，可能情況是新增本機異動時，馬上就取得遠端異動，而本機異動尚未反應到遠端異動。
                        else if (LocalEntry.Count > Entry.Count)
                        {
                            Console.WriteLine("狀況二");
                            LocalEntry.Count -= Entry.Count; //保留ClientChangeEntry，消去ServerChangeEntry
                        }
                        //狀況三：本機異動（ClientChangeEntry）小於遠端異動（ServerChangeEntry）的異動筆數（Count）：
                        //1.遠端異動（ServerChangeEntry）加入到實際異動（GetChangeSet的傳回值），並且減去本機異動的異動筆數。
                        //2.本機異動（ClientChangeEntry）移除。
                        //3.發生情況：當新增本機異動至取得遠端異動期間，還有其他機器對於此筆資料做修改。
                        else if (LocalEntry.Count < Entry.Count)
                        {
                            Console.WriteLine("狀況三");

                            Entry.Count -= LocalEntry.Count;

                            RemoveEntries.Add(LocalEntry); //將ClientChangeEntry移除

                            ChangeEntries.Add(Entry); //將ServerChangeEntry加入到ChangeSet當中
                        }
                    }

                    //將ClientEntry自ClientChangeSet當中移除
                    foreach (ChangeEntry RemoveEntry in RemoveEntries)
                        mClientChangeSet[UID].Remove(RemoveEntry);
                }
                //本機沒有異動，但是遠端有異動的情況
                else
                {
                    Console.WriteLine("狀況四");
                    ChangeEntries.Add(Entry);
                }

                //不管是哪種狀況，都要將Sequence的值設為最大值，下次才不會重覆取得
                if (Entry.Sequence > CurrentSequence)
                    CurrentSequence = Entry.Sequence;
            }

            return ChangeEntries;
        }

        /// <summary>
        /// 設定Client的ChangeSet
        /// </summary>
        /// <param name="ClientChangeSet"></param>
        public void SetClientChangeSet(IEnumerable<ChangeEntry> ClientChangeSet)
        {
            foreach (ChangeEntry Entry in ClientChangeSet)
            {
                string UID = Entry.TableName +"-"+Entry.Action.ToString()+ "-" + Entry.DataID;

                if (!mClientChangeSet.ContainsKey(UID))
                    mClientChangeSet.Add(UID, new List<ChangeEntry>());

                mClientChangeSet[UID].Add(Entry);
            }
        }

        /// <summary>
        /// BaseLine的決定是取得資料庫最後一筆更新的資料為BaseLine。
        /// </summary>
        public void SetBaseLine()
        {
            DSXmlHelper rsp = DSAServices.CallService(GETLASTSEQUENCE_SERVICENAME, new DSRequest()).GetContent();
            int sequence = 0;

            if (int.TryParse(rsp.GetText("@Sequence"), out sequence))
                CurrentSequence = sequence;
        }

        public void SetBaseLine(long sequence)
        {
            CurrentSequence = sequence;
        }

        #endregion
    }
}