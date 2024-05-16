using RimWorld;
using System.Linq;
using System.Reflection;

namespace Defaults.Storyteller
{
    public class DifficultySub : Difficulty
    {
        public Difficulty GetDifficultyValues()
        {
            Difficulty difficulty = new Difficulty();
            foreach (FieldInfo field in typeof(Difficulty).GetFields().Where(f => !f.IsLiteral))
            {
                field.SetValue(difficulty, field.GetValue(this));
            }
            return difficulty;
        }
    }
}
