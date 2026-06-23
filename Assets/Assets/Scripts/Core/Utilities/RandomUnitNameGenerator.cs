using UnityEngine;

public static class RandomUnitNameGenerator
{
    private static readonly string[] Names =
    {
        "Aria",
        "Borin",
        "Cyrus",
        "Dorian",
        "Elara",
        "Fenris",
        "Gwen",
        "Hector",
        "Iris",
        "Juno",
        "Kael",
        "Luna",
        "Magnus",
        "Nyx",
        "Orion",
        "Riven",
        "Selene",
        "Thorne",
        "Vex",
        "Zephyr"
    };

    public static string Generate(
        Team team)
    {
        string name =
            Names[
                Random.Range(
                    0,
                    Names.Length)];

        return $"{name} ({team})";
    }
}