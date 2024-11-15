using System.Reflection;
using TextBlade.Core.Locations;
using TextBlade.Core.UserCode;

namespace TextBlade.Core.Game;

public static class CodeBehindRunner
{
    private static Dictionary<string, Type> _classNameToType = new();

    static CodeBehindRunner()
    {
        // Cache all class names and types. Maybe a bad idea. How much of the game will the player really see?
        var assembly = Assembly.GetEntryAssembly();
        RegisterAssemblyClasses(assembly);
    }

    // Used in cases like unit testing where we can't control the entry assembly
    public static void RegisterAssemblyClasses(Assembly assembly)
    {
        var classes = assembly.GetTypes().Where(c => c.IsSubclassOf(typeof(LocationCodeBehind)));
        foreach (var clazz in classes)
        {
            var className = clazz.Name;
            if (_classNameToType.ContainsKey(className))
            {
                throw new InvalidOperationException($"There are multiple LocationCodeBehind classes defined with the name {className}!");
            }

            _classNameToType[className] = clazz;
        }
    }

    public static void ExecuteLocationCode(Location currentLocation)
    {
        var className = currentLocation.LocationClass;
        if (className == null)
        {
            // No class name specified. Nothing to do.
            return;
        }
        
        if (string.IsNullOrWhiteSpace(className))
        {
            throw new ArgumentException($"{currentLocation.Name} has an empty LocationClass attribute!");
        }

        if (!_classNameToType.ContainsKey(className))
        {
            List<string> allLocationClasses = _classNameToType.Keys.ToList();
            allLocationClasses.Sort();
            throw new ArgumentException($"Looks like {currentLocation.Name} has a LocationClass of {className}, but TextBlade can't find the class. Make sure you add the [LocationCode] attribute to your class, and make sure it's outside the `Content` directory. TextBlade knows about these classes: {String.Join(", ", allLocationClasses)}");
        }

        // TODO: document this. We create one instance PER VISIT TO THIS CLASS. And this instance DOES NOT PERSIST.
        // So it has to be ... stateless? I forget the word right this minute. 
        // And, it must have a parameterless constructor. That does whatever it needs to.
        var type = _classNameToType[className];
        var instance = Activator.CreateInstance(type) as LocationCodeBehind;
        instance.BeforeShowingLocation(currentLocation);
    }
}
