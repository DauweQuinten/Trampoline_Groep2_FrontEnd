using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameVariablesHolder
{
    public static string testString { get; set; }

    public static int[] playerMapping = { -1, -1 };

    public static int score { get; set; }
    public static string Username { get; set; }
    public static int Score { get; set; } = 115;
    public static int Id { get; set; }
}