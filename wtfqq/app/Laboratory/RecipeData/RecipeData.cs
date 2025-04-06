namespace wtfqq.app.Laboratory.RecipeData;

public class Recipe(
    string title,
    string description,
    List<string> ingredients,
    List<string> instructions,
    TimeSpan preparationTime,
    int servings,
    string category)
{
    public string Title { get; } = title;
    private string Description { get; } = description;
    private List<string> Ingredients { get; set; } = ingredients;
    private List<string> Instructions { get; set; } = instructions;
    public TimeSpan PreparationTime { get; } = preparationTime;
    private int Servings { get; } = servings;
    public string Category { get; } = category;

    public override string ToString()
    {
        return
            $"| Title: {Title} | " +
            $"Category: {Category} | " +
            $"Preparation Time: {PreparationTime.TotalMinutes} minutes |";
    }
}