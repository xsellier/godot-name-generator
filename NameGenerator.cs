using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class NameGenerator
{
    private static readonly Dictionary<String, List<string>> Letters = new Dictionary<String, List<string>>()
    {
        {
            "VOYELLE", new List<string>(){"a", "e", "i", "o", "u", "y"}
        },
        {
            "DOUBLE_VOYELLE",
            new List<string>() {"au", "oa", "ou", "ie", "ae", "eu"}},

        {
            "CONSONNE",
            new List<string>() {"b", "c", "d", "f", "g", "h", "j", "l", "m", "n", "p", "r", "s", "t", "v", "w", "x", "z"}
        },
        {
            "DOUBLE_CONSONNE",
            new List<string>() {"mm", "nn", "st", "ch", "ll", "tt", "ss"}},

        {
            "COMPOSE",
            new List<string>() {"gu", "cc", "sc", "tr", "fr", "pr", "br", "cr", "ch", "gn", "ix", "an", "do", "ir", "as"}
        }
    };

    private static readonly Dictionary<String, List<String>> Transition = new Dictionary<String, List<string>>
    {
        {"INITIAL", new List<string>() {"VOYELLE", "CONSONNE", "COMPOSE"}},

        {"VOYELLE", new List<string>() {"CONSONNE", "DOUBLE_CONSONNE", "COMPOSE"}},
        {"DOUBLE_VOYELLE", new List<string>() {"CONSONNE", "DOUBLE_CONSONNE", "COMPOSE"}},

        {"CONSONNE", new List<string>() {"VOYELLE", "DOUBLE_VOYELLE"}},
        {"DOUBLE_CONSONNE", new List<string>() {"VOYELLE", "DOUBLE_VOYELLE"}},

        {"COMPOSE", new List<string>() {"VOYELLE"}}
    };

    private static int PickRandomNumber(int maxValue, int minValue = 0)
    {
        return (int) (GD.Randi() % (maxValue - minValue) + minValue);
    }

    public static List<String> CloneArray(List<String> original)
    {
        var result = new List<String>();
        result.AddRange(original);
        return result;
    }

    private static List<String> GetLetter(string state, int maxLength)
    {
        List<String> transitions = CloneArray(Transition[state]);

        if (maxLength < 3)
        {
            transitions.Remove("COMPOSE");
            transitions.Remove("DOUBLE_CONSONNE");
            transitions.Remove("DOUBLE_VOYELLE");
        }

        var stateIndex = PickRandomNumber(transitions.Count);
        state = transitions[stateIndex];

        List<String> lettersList = Letters[state];
        int letterIndex = PickRandomNumber(lettersList.Count);

        List<String> result = new List<string> {state, lettersList[letterIndex]};
        return result;
    }

    public static string Generate(int minLength = 3, int maxLength = 12)
    {
        var length = PickRandomNumber(maxLength, minLength);
        String name = "";
        int index = 0;
        String state = "INITIAL";


        while (index < length)
        {
            var obj = GetLetter(state, length - index);
            state = obj[0];
            var lastLetter = obj[1];

            name += lastLetter;
            index += lastLetter.Length;
        }

        return Char.ToUpper(name[0]) + name.Substring(1);
    }
} 