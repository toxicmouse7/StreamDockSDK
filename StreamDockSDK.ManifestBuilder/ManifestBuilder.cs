using StreamDockSDK.Attributes;

namespace StreamDockSDK.ManifestBuilder;

public class ManifestBuilder<TAssembly>
{
    public Manifest Build(
        string author,
        string category,
        string name,
        string version,
        string description,
        string icon,
        IEnumerable<SupportedOperatingSystem> supportedOS)
    {
        var manifestActions = GetActionsManifest();
        var assembly = typeof(TAssembly).Assembly;
        var codePath = Path.ChangeExtension(Path.GetFileName(assembly.Location), "exe");


        return new Manifest
        {
            Author = author,
            Category = category,
            Name = name,
            Version = version,
            Description = description,
            Icon = icon,
            CodePath = codePath,
            Actions = manifestActions!,
            OS = supportedOS
        };
    }

    private static List<Action> GetActionsManifest()
    {
        var actions = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
            .Where(t => typeof(AbstractAction).IsAssignableFrom(t) && !t.IsAbstract);

        var manifestActions = actions.Select(a =>
        {
            if (Attribute.GetCustomAttribute(a, typeof(ActionAttribute)) is not ActionAttribute actionAttribute)
            {
                return null;
            }

            var states = (IEnumerable<StateAttribute>)Attribute.GetCustomAttributes(a, typeof(StateAttribute));

            return new Action
            {
                Name = actionAttribute.Name,
                Icon = actionAttribute.IconPath,
                PropertyInspectorPath = actionAttribute.PropertyInspectorPath,
                SupportedInMultiActions = actionAttribute.SupportedInMultiActions,
                Tooltip = actionAttribute.Tooltip,
                UserTitleEnabled = actionAttribute.UserTitleEnabled,
                UUID = a.FullName!,
                States = states.Select(s => new State
                {
                    Image = s.Image
                })
            };
        }).Where(x => x is not null).ToList();

        return manifestActions!;
    }
}