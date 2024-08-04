using System;
using System.Collections.Generic;
using Godot;
using ProjectBriseis.Scripts.AutoLoad;

namespace ProjectBriseis.Scripts.Util;

public abstract class RandomUtil {
    private static Random rnd = new Random();

    public static List<int> GenerateRandomNumbers(int from, int to, int amount) {
        if (to - from + 1 < amount) {
            Log.Error("Can't generate enough numbers in the provided range");
            throw new Exception("Can't generate enough numbers in the provided range");
        }

        List<int> randomNumbers = new List<int>();
        List<int> available = new List<int>();

        // Populate available list with all numbers from "from" to "to"
        for (int i = from; i <= to; i++) {
            available.Add(i);
        }

        // Generate "amount" of random numbers without repetition
        for (int i = 0; i < amount; i++) {
            int randomIdx = rnd.Next(0, available.Count);
            int randomNumber = available[randomIdx];
            available.RemoveAt(randomIdx);
            randomNumbers.Add(randomNumber);
        }

        return randomNumbers;
    }

}