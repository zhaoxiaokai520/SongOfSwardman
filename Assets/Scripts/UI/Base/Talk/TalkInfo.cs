using System.Collections.Generic;

namespace Assets.Scripts.UI.Base.Talk
{
    public class TalkInfo
    {
        public int talkId = 0;
        int index = 0;//sentence talk circle index to loop
        public List<TalkSentence> sentences = new List<TalkSentence>();

        public string GetContent(string type = "default")
        {
            if (-1 == index)
            {
                index = 0;
                return "";
            }

            for (int i = 0; i < sentences.Count; i++)
            {
                TalkSentence sent = sentences[i];
                if (sent.type.Equals(type))
                {
                    if (index < sent.texts.Count)
                    {
                        string ret = sent.texts[index++];
                        if (index >= sent.texts.Count)
                        {
                            index = -1;
                        }
                        return ret;
                    }
                }
            }

            return "not found talk content error!!";
        }
    }

    public class TalkSentence
    {
        public string type = "";
        public int fontSize = 12;
        public List<string> texts = new List<string>();
    }
}
