using System.Collections.Generic;

public class StateMachine
{
    private List<AbstractStateBase> listStates;
    private AbstractStateBase currentState;


    public StateMachine()
    {
        listStates = new List<AbstractStateBase>();
        InitializeStateMachine();
    }

    public StateMachine(AbstractStateBase newState)
    {
        listStates = new List<AbstractStateBase>();
        currentState = newState;
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        // Stateler için sadece Base construstorla yapmaya bak
        AddState(new MainMenuState());
        AddState(new LevelPlayingState());
        AddState(new LevelEndWinState());
        AddState(new LevelEndLoseState());
        AddState(new BonusLevelPlayingState());
        AddState(new LevelEndBonusState());

        currentState = listStates[0];
    }

    public void AddState(AbstractStateBase newState)
    {
        listStates.Add(newState);
    }

    public void RemoveState(AbstractStateBase state)
    {
        listStates.Remove(state);
    }

    public void ChangeState(States state)
    {
        currentState?.ExitState(); 
        currentState = listStates[(int) state];
        currentState.EnterState();
    }


    




}
