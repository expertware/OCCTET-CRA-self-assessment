namespace exp.Models.Helpers
{
    public class Enums
    {
        public static class QuestionTypes
        {
            private const string score = "Score";
            private const string check = "Check";
            private const string text = "Text";

            public static string Check => check;
            public static string Text => text;
            public static string Score => score;
        }
        public static class SurveyTypes
        {
            private const string score = "score";
            private const string ruleCheck = "rule_check";

            public static string RuleCheck => ruleCheck;
            public static string Score => score;
        }
        public static class CraSurvey
        {
            private const int id = 3;
            private const string positiveAnswer = "Your SME is in the scope of CRA regulation";
            private const string negativeAnswer = "Your SME is not in the scope of CRA regulation";

            public static string PositiveAnswer => positiveAnswer;
            public static string NegativeAnswer => negativeAnswer;
            public static int Id => id;
        }
        public static class SurveysEnum
        {
            private const int awarenessId = 3;
            private const int selfQualificationId = 7;
            private const int maturityId = 8;

            public static int AwarenessId = awarenessId;
            public static int SelfQualificationId = selfQualificationId;
            public static int MaturityId = maturityId;
        }
        public static class CompanySizeLabels
        {
            public static readonly List<string> Labels = new List<string>
            {
                {  "Micro (1–9)" },
                {  "Small (10–49)" },
                {  "Medium (50–249)" },
                {  "Large (250+)" }
            };
        }
        public static class QuestionForReport
        {
            private const int craFamiliarityId = 51;
            
            public static int CraFamiliarityId  = craFamiliarityId; 
        }

    }
}
