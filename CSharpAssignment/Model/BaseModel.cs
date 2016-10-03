// Copyright(c) Daniel Veintimilla 2016.

namespace CSharpAssignment.Model
{
    /// <summary>Represents the base model class. Models extends this class.</summary>
    public class BaseModel
    {
        public int ItemId { get; set; }

        public BaseModel()
        {
            ItemId = -1;
        }
    }
}