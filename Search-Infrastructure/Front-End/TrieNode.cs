using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Front_End
{
    public class TrieNode
    {
        public TrieNode[] children;
        public char value;
        public bool isWord;
        public TrieNode(char value)
        {
            this.value = value;
            this.isWord = false;
            children = new TrieNode[256];
        }

        public void AddChild(char c)
        {
            this.children[c] = new TrieNode(c);
        }

        public bool HasChild(char c)
        {
            return this.children[c] != null;
        }

        public TrieNode GetChild(char c)
        {
            return this.children[c];
        }

        public void SetAsWord()
        {
            isWord = true;
        }
    }
}