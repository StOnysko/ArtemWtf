namespace wtfqq.app.Laboratory.RecipeData;

public static class RecipeManager
{
    private static readonly Random Random = new();

    private static readonly List<string> SampleTitles =
    [
        "Pasta", "Chicken Curry", "Vegetable Stir Fry",
        "Beef Stroganoff", "Tomato Soup", "Pancakes", "Grilled Salmon"
    ];

    private static readonly List<string> SampleDescriptions =
    [
        "A delicious and easy-to-make dish.",
        "Perfect for a cozy evening meal.",
        "Healthy and quick to prepare.",
        "A rich and creamy classic.",
        "Comfort food for any time of year."
    ];

    private static readonly List<string> SampleIngredients =
    [
        "Chicken", "Beef", "Tomato", "Onion", "Garlic", "Carrot", "Pasta",
        "Rice", "Milk", "Eggs", "Flour", "Butter", "Sugar", "Salt", "Pepper"
    ];

    private static readonly List<string> SampleInstructions =
    [
        "Chop the vegetables.",
        "Heat the oil in a pan.",
        "Add ingredients and stir well.",
        "Simmer for 20 minutes.",
        "Serve hot and enjoy!"
    ];

    private static readonly List<string> SampleCategories =
    [
        "Dinner", "Lunch", "Breakfast", "Snack", "Dessert", "Soup"
    ];

    private static Recipe GenerateRandomRecipe()
    {
        var title = SampleTitles[Random.Next(SampleTitles.Count)];
        var description = SampleDescriptions[Random.Next(SampleDescriptions.Count)];
        var ingredients = SampleIngredients.OrderBy(_ => Random.Next()).Take(Random.Next(3, 7)).ToList();
        var instructions = SampleInstructions.OrderBy(_ => Random.Next()).Take(Random.Next(2, 5)).ToList();
        var prepTime = TimeSpan.FromMinutes(Random.Next(10, 60));
        var servings = Random.Next(1, 6);
        var category = SampleCategories[Random.Next(SampleCategories.Count)];

        return new Recipe(title, description, ingredients, instructions, prepTime, servings, category);
    }
    
    public static List<Recipe> RecipeList(int count)
    {
        var recipes = new List<Recipe>();
        for (var i = 0; i < count; i++)
        {
            recipes.Add(GenerateRandomRecipe());
        }
        return recipes;
    }
    
    public static List<Recipe> SortRecipes(List<Recipe> recipes)
    {
        return recipes
            .OrderBy(r => r.Title)
            .ThenBy(r => r.Category)
            .ThenBy(r => r.PreparationTime)
            .ToList();
    }
}
