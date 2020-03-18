using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Front_End
{
    public class Trie
    {
        public TrieNode Root;
        public Trie()
        {
            Root = new TrieNode('#');
        }

        public void AddTitle(string word)
        {
            TrieNode node = Root;
            foreach (char c in word)
            {
                if (!node.HasChild(c))
                {
                    node.AddChild(c);
                }
                node = node.GetChild(c);
            }
            node.SetAsWord();
        }

        public List<string> GetWordsWithPrefix(string prefix)
        {
            List<string> res = new List<string>();
            TrieNode node = Root;
            foreach (char c in prefix)
            {
                if (!node.HasChild(c))
                {
                    return res;
                }
                node = node.GetChild(c);
            }
            GetWordsWithPrefix(node, prefix, "", res);
            return res;
        }

        public void GetWordsWithPrefix(TrieNode node, string prefix, string temp, List<string> res)
        {
            if (node == null) return;
            if (node.isWord)
            {
                res.Add(prefix + temp);
            }
            foreach (TrieNode child in node.children)
            {
                if (child != null)
                {
                    GetWordsWithPrefix(child, prefix, temp + child.value, res);
                }
            }
        }
    }
}