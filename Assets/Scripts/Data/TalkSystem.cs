using Mono.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class TalkSystem : Singleton<TalkSystem>
    {
        Dictionary<int, TalkInfo> mTalkDic = new Dictionary<int, TalkInfo>();
        public void LoadTalkData()
        {
            _loadTalk("NPC_1");
            _loadTalk("NPC_2");
        }

        public string GetTalk(int talkeeId, int talkerId)
        {
            if (mTalkDic.ContainsKey(talkeeId))
            {
                return mTalkDic[talkeeId].GetContent();
            }

            return string.Format("error not found talk {0} say to {1}", talkerId, talkeeId);
        }

        private static void Read()
        {

        }

        private void _loadTalk(string name)
        {
            string editorPath = Application.dataPath + "/RawRes/" + name + ".xml";
            if (System.IO.File.Exists(editorPath))
            {
                TalkInfo info = new TalkInfo();
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(editorPath);
                    XmlNode node = doc.SelectSingleNode("Talk");
                    int result = 0;
                    int.TryParse(node.Attributes["id"].Value, out result);
                    info.talkId = result;
                    XmlNodeList nodes = node.ChildNodes;
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        if (nodes[i].NodeType == XmlNodeType.Element)
                        {
                            //XmlNode snode = nodes[i].SelectSingleNode("Sentence");
                            XmlNode snode = nodes[i];
                            TalkSentence sent = new TalkSentence();
                            sent.type = snode.Attributes["type"].Value;
                            XmlNodeList cnodes = snode.ChildNodes;
                            for (int j = 0; j < cnodes.Count; j++)
                            {
                                XmlNode cnode = cnodes[j];
                                sent.texts.Add(cnode.InnerText);
                            }
                            info.sentences.Add(sent);
                        }
                    }

                    mTalkDic.Add(info.talkId, info);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}