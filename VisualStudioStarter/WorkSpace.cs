using VisualStudioStarter.Utils;

namespace VisualStudioStarter;

public class WorkSpace()
{
    public String Path { get; set; } = String.Empty;
    public Boolean Active { get; set; }
    public WorkSpaceType Type { get; set; }
    public List<Solution> Solutions { get; set; } = [];

    public WorkSpace(WorkSpaceType type) : this()
    {
        Type = type;

        switch (Type)
        {
            case WorkSpaceType.Standard:
                break;
            case WorkSpaceType.AddNew:
                Path = "-- Add new --";
                break;
            case WorkSpaceType.Blank:
                Path = String.Empty;
                break;
        }
    }
}