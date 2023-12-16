using Microsoft.AspNetCore.Components;

namespace Presentation.Components;

public partial class ComponentTest : ComponentBase
{
    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
    }
}