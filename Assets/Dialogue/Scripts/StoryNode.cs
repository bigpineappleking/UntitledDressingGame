using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStoryNode", menuName = "Dressing/Story Node")]
public class StoryNode : ScriptableObject
{
    public string code;
    [TextArea(10,50)]
    public string story;
    public StoryNode defaultStory;
    public List<StoryNode> storyBranches = new List<StoryNode>();
}
