using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DialogueEntity;

public interface IDialogEvent
{
    void OnEachNode(Node node);

    void End();
}



