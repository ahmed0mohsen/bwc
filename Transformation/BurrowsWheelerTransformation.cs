﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transformation
{
    public class BurrowsWheelerTransformation
    {
        private class Suffixe
        {
            public long HashValue { get; set; }
            public long Index { get; set; }
            public long Mods { get;set }
        }
        private string text;
        private string transformedText;
        private int originalTextIndex;
        private int[] next;
        private long mod;
        private long _base;
        public BurrowsWheelerTransformation()
        {
            mod = 10000000+7;
            @base = 1000+7;
        }

        private void Initialize(string _text)
        {
            if (_text.Length == 0)
                throw new Exception("The input string is empty!");

            this.text = _text;
            next = new int[_text.Length];
        }

        private string[] GenerateSuffixes()
        {
            string[] suffixes = new string[text.Length];
            suffixes[0] = text;

            for (int i = 1; i < text.Length; i++)
            {
                suffixes[i] = suffixes[i - 1].Substring(1);
                suffixes[i] += suffixes[i - 1][0];
            }

            return suffixes;
        }

        private void MergeSortSuffixes(string[] suffixes)
        {
            MergeSortSuffixes(suffixes, 0, suffixes.Length - 1);
        }

        private void MergeSortSuffixes(string[] suffixes, int start, int end)
        {
            if (start >= end)
                return;

            int mid = (start + end) / 2;

            MergeSortSuffixes(suffixes, start, mid);
            MergeSortSuffixes(suffixes, mid + 1, end);

            string[] sorted = new string[end - start + 1];

            int leftIndex = start; //Iterates over the left array.
            int rightIndex = mid + 1; //Iterates over the right array.
            int curInd = 0; //Iterates over the sorted merged array.

            while (leftIndex <= mid && rightIndex <= end)
            {
                if (suffixes[leftIndex].CompareTo(suffixes[rightIndex]) < 0)
                    sorted[curInd] = suffixes[leftIndex++];
                else
                    sorted[curInd] = suffixes[rightIndex++];

                curInd++;
            }

            while (leftIndex <= mid)
            {
                sorted[curInd++] = suffixes[leftIndex++];
            }

            while (rightIndex <= end)
            {
                sorted[curInd++] = suffixes[rightIndex++];
            }

            for (int i = 0; i < sorted.Length; i++)
            {
                suffixes[i + start] = sorted[i];
            }
        }

        private int[] GetNextArray(string[] originalSuffixes, string[] sortedSuffixes)
        {
            int prev = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (sortedSuffixes[i] == originalSuffixes[0])
                {
                    originalTextIndex = i;
                    break;
                }
            }

            prev = originalTextIndex;

            for (int i = 1; i < text.Length; i++)
            {
                for (int j = 0; j < text.Length; j++)
                {
                    if (sortedSuffixes[j] == originalSuffixes[i])
                    {
                        next[prev] = j;
                        prev = j;
                        break;
                    }
                }
            }

            next[prev] = originalTextIndex;

            return next;
        }

        public string Transform(string _text)
        {
            Initialize(_text);
            string[] suffixes = GenerateSuffixes();
            string[] sortedSuffixes = new string[suffixes.Length];
            suffixes.CopyTo(sortedSuffixes, 0);
            MergeSortSuffixes(sortedSuffixes);

            int[] next = GetNextArray(suffixes, sortedSuffixes);
            string output = "";
            for (int i = 0; i < text.Length; i++)
            {
                output += sortedSuffixes[i][text.Length - 1];
            }

            this.transformedText = output;
            return output;
        }

        public string InverseTransformation(string _text)
        {
            this.transformedText = _text;
            int index = next[originalTextIndex];
            string inversed = "";
            inversed += transformedText[index];
            for (int i = 0; i < next.Length - 1; i++)
            {
                index = next[index];
                inversed += transformedText[index];
            }
            return inversed;
        }

        private int BinarySearch(string[] arr, string elem)
        {
            return BinarySearch(arr, elem, 0, arr.Length - 1);
        }
        
        private int BinarySearch(string[] arr, string elem, int start, int end)
        {
            while (start < end)
            {
                int mid = (start + end) / 2;

                if (arr[mid] == elem)
                    return mid;
                else if (arr[mid].CompareTo(elem) < 0)
                    start = mid + 1;
                else
                    end = mid - 1;
            }
            return -1;
        }

        private List<Suffixe> Hash(string text)
        {

        }
    }
}
