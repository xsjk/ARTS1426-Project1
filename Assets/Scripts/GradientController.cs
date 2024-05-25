using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;

public class GradientController : MonoBehaviour
{
    public GameObject slots;

    private Dictionary<int, Color> colors = new Dictionary<int, Color>();
    private LinkedListNode<int>[] nodes = new LinkedListNode<int>[8];
    private LinkedList<int> intervals = new LinkedList<int>();

    private void Start() {
        foreach (Transform child in slots.transform)
            child.gameObject.GetComponent<ColideTriggerLogic>().controller = this;
    }
    public void Add(float angle, Color color) {
        float rate = (angle / 2.0f / Mathf.PI) + 0.5f; 
        int index = (int)(rate * 8);
        Debug.Log($"Add color {color} at {index}");
        colors[index] = color;

        if (intervals.Count == 0) {
            var new_node = intervals.AddLast(index);
            for (int i = 0; i < 8; i++)
                nodes[i] = new_node;
        } else {
            var prev_node = nodes[index];
            var next_node = prev_node.Next;
            if (next_node == null)
                next_node = intervals.First;

            var new_node = intervals.AddAfter(prev_node, index);
            var next_idx = next_node.Value;

            for (int j = index; j != next_idx; j = (j + 1) % 8)
                nodes[j] = new_node;
        }
        // Debug.Log($"nodes: {nodes[0].Value}, {nodes[1].Value}, {nodes[2].Value}, {nodes[3].Value}, {nodes[4].Value}, {nodes[5].Value}, {nodes[6].Value}, {nodes[7].Value}  Intervals: {string.Join(", ", intervals)}");
    }
    
    public void Remove(float angle) {
        float rate = (angle / 2.0f / Mathf.PI) + 0.5f;
        int index = (int)(rate * 8);
        Debug.Log($"Remove color at {index}");

        if (intervals.Count == 1) {
            intervals.Clear();
            colors.Clear();
            for (int i = 0; i < 8; i++)
                nodes[i] = null;
        } else {
            var node = nodes[index];
            var prev_node = node.Previous;
            var next_node = node.Next;
            if (prev_node == null)
                prev_node = intervals.Last;
            if (next_node == null)
                next_node = intervals.First;

            intervals.Remove(node);
            colors.Remove(index);

            for (int j = index; j != next_node.Value; j = (j + 1) % 8)
                nodes[j] = prev_node;
        }
        // Debug.Log($"nodes: {nodes[0].Value}, {nodes[1].Value}, {nodes[2].Value}, {nodes[3].Value}, {nodes[4].Value}, {nodes[5].Value}, {nodes[6].Value}, {nodes[7].Value}  Intervals: {string.Join(", ", intervals)}");
    }

    public Color Evaluate(float rate) {
        if (colors.Count == 0)
            return Color.black;
        else if (colors.Count == 1)
            return colors[nodes[0].Value];

        int index = Mathf.RoundToInt(rate * 8) - 1;
        if (index < 0)
            index += 8;
        LinkedListNode<int> leftNode = nodes[index];
        LinkedListNode<int> rightNode = leftNode.Next;
        if (rightNode == null)
            rightNode = intervals.First;

        Color leftColor = colors[leftNode.Value];
        Color rightColor = colors[rightNode.Value];
        float leftRate = (leftNode.Value + 0.5f) / 8.0f;
        float rightRate = (rightNode.Value + 0.5f) / 8.0f;

        if (rightRate <= leftRate)
            rightRate += 1.0f;
        if (rate < leftRate)
            rate += 1.0f;

        float t = (rate - leftRate) / (rightRate - leftRate);

        // Debug.Log($"t={t} rate={origin_rate} left={leftRate} right={rightRate}");
        return Color.Lerp(leftColor, rightColor, t);
    }


}
