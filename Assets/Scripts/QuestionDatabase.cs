using UnityEngine;

[CreateAssetMenu(fileName = "QuestionDatabase",
                 menuName = "FrogJump/Question Database")]
public class QuestionDatabase : ScriptableObject
{
    public QuestionData[] questions;

    // Call this to get all questions loaded from code
    public void LoadQuestions()
    {
        questions = new QuestionData[]
        {
            // ── Group 1: Symbols ──────────────────
            new QuestionData {
                questionText = "What is the chemical symbol for Gold?",
                answers = new string[] { "Ag", "Au", "Fe", "Cu" },
                correctIndex = 1
            },
            new QuestionData {
                questionText = "What is the chemical symbol for Iron?",
                answers = new string[] { "Ir", "In", "Fe", "Fr" },
                correctIndex = 2
            },
            new QuestionData {
                questionText = "What is the chemical symbol for Silver?",
                answers = new string[] { "Si", "Ag", "Al", "Sr" },
                correctIndex = 1
            },
            new QuestionData {
                questionText = "What is the chemical symbol for Sodium?",
                answers = new string[] { "So", "Sd", "Na", "Ni" },
                correctIndex = 2
            },
            new QuestionData {
                questionText = "What is the chemical symbol for Potassium?",
                answers = new string[] { "Po", "Pt", "Ks", "K" },
                correctIndex = 3
            },
            new QuestionData {
                questionText = "What is the chemical symbol for Lead?",
                answers = new string[] { "Le", "Ld", "Pb", "Pl" },
                correctIndex = 2
            },
            new QuestionData {
                questionText = "What is the chemical symbol for Mercury?",
                answers = new string[] { "Me", "Mg", "Mn", "Hg" },
                correctIndex = 3
            },
            new QuestionData {
                questionText = "What is the chemical symbol for Copper?",
                answers = new string[] { "Co", "Cp", "Cu", "Cr" },
                correctIndex = 2
            },
            new QuestionData {
                questionText = "What is the chemical symbol for Tungsten?",
                answers = new string[] { "Tu", "Tg", "Tn", "W" },
                correctIndex = 3
            },
            new QuestionData {
                questionText = "What is the chemical symbol for Tin?",
                answers = new string[] { "Ti", "Tn", "Sn", "St" },
                correctIndex = 2
            },

            // ── Group 2: Atomic Numbers ───────────
            new QuestionData {
                questionText = "What is the atomic number of Carbon?",
                answers = new string[] { "4", "8", "6", "12" },
                correctIndex = 2
            },
            new QuestionData {
                questionText = "What is the atomic number of Hydrogen?",
                answers = new string[] { "2", "1", "3", "4" },
                correctIndex = 1
            },
            new QuestionData {
                questionText = "What is the atomic number of Oxygen?",
                answers = new string[] { "6", "10", "8", "12" },
                correctIndex = 2
            },
            new QuestionData {
                questionText = "What is the atomic number of Nitrogen?",
                answers = new string[] { "5", "7", "9", "11" },
                correctIndex = 1
            },
            new QuestionData {
                questionText = "What is the atomic number of Helium?",
                answers = new string[] { "1", "3", "4", "2" },
                correctIndex = 3
            },
            new QuestionData {
                questionText = "What is the atomic number of Gold?",
                answers = new string[] { "79", "47", "29", "82" },
                correctIndex = 0
            },
            new QuestionData {
                questionText = "What is the atomic number of Uranium?",
                answers = new string[] { "82", "90", "92", "94" },
                correctIndex = 2
            },
            new QuestionData {
                questionText = "What is the atomic number of Iron?",
                answers = new string[] { "24", "26", "28", "30" },
                correctIndex = 1
            },

            // ── Group 3: Element Properties ───────
            new QuestionData {
                questionText = "Which element is the lightest of all?",
                answers = new string[] { "Helium", "Oxygen", "Hydrogen", "Lithium" },
                correctIndex = 2
            },
            new QuestionData {
                questionText = "Which of these is a Noble Gas?",
                answers = new string[] { "Oxygen", "Chlorine", "Argon", "Nitrogen" },
                correctIndex = 2
            },
            new QuestionData {
                questionText = "Which element is liquid at room temperature?",
                answers = new string[] { "Gold", "Mercury", "Lead", "Iron" },
                correctIndex = 1
            },
            new QuestionData {
                questionText = "Which element has the highest melting point?",
                answers = new string[] { "Iron", "Gold", "Tungsten", "Carbon" },
                correctIndex = 2
            },
            new QuestionData {
                questionText = "Which element is most abundant in Earth's crust?",
                answers = new string[] { "Silicon", "Iron", "Oxygen", "Aluminium" },
                correctIndex = 2
            },
            new QuestionData {
                questionText = "Which element is most abundant in the universe?",
                answers = new string[] { "Helium", "Hydrogen", "Carbon", "Oxygen" },
                correctIndex = 1
            },
            new QuestionData {
                questionText = "How many elements are in the periodic table?",
                answers = new string[] { "108", "116", "118", "120" },
                correctIndex = 2
            },
            new QuestionData {
                questionText = "Which element is used in pencils?",
                answers = new string[] { "Carbon", "Lead", "Graphite", "Silicon" },
                correctIndex = 0
            },

            // ── Group 4: Groups and Periods ───────
            new QuestionData {
                questionText = "Which group are the Alkali metals in?",
                answers = new string[] { "Group 2", "Group 1", "Group 7", "Group 0" },
                correctIndex = 1
            },
            new QuestionData {
                questionText = "Which group are the Halogens in?",
                answers = new string[] { "Group 1", "Group 6", "Group 17", "Group 18" },
                correctIndex = 2
            },
            new QuestionData {
                questionText = "Which group are Noble Gases in?",
                answers = new string[] { "Group 1", "Group 17", "Group 16", "Group 18" },
                correctIndex = 3
            },
            new QuestionData {
                questionText = "How many periods are in the periodic table?",
                answers = new string[] { "5", "6", "7", "8" },
                correctIndex = 2
            },

            // ── Group 5: Fun Facts ─────────────────
            new QuestionData {
                questionText = "Which element is essential for human bones?",
                answers = new string[] { "Iron", "Calcium", "Potassium", "Zinc" },
                correctIndex = 1
            },
            new QuestionData {
                questionText = "Which element is used in fluorescent lights?",
                answers = new string[] { "Neon", "Argon", "Mercury", "Xenon" },
                correctIndex = 0
            },
            new QuestionData {
                questionText = "Which element makes up most of the air we breathe?",
                answers = new string[] { "Oxygen", "Carbon Dioxide", "Nitrogen", "Argon" },
                correctIndex = 2
            },
            new QuestionData {
                questionText = "Which element is used in batteries?",
                answers = new string[] { "Lithium", "Sodium", "Potassium", "Calcium" },
                correctIndex = 0
            },
            new QuestionData {
                questionText = "Which element is radioactive?",
                answers = new string[] { "Gold", "Silver", "Uranium", "Platinum" },
                correctIndex = 2
            },
        };
    }
}