using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceUI : MonoBehaviour
{
    [SerializeField] private TMP_Text systemClockTMP;

    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;

    [SerializeField] private TMP_Text pauseButtonText;
    [SerializeField] private TMP_Text resumeButtonText;

    [SerializeField] private Button x1Button;
    [SerializeField] private Button x2Button;
    [SerializeField] private Button x5Button;
    [SerializeField] private Button x10Button;
    [SerializeField] private Button x15Button;
    [SerializeField] private Button x20Button;

    private SimulationClock simulationClockReference;

    // Start is called before the first frame update
    void Start()
    {
        simulationClockReference = SimulationClock.GetInstance();

        //Check against null ref.
        if(simulationClockReference == null)
        {
            Debug.LogError("The reference to the simulation clock is null! Have you called get reference in the correct place?");

            return;
        }

        //Add UI listeners here.
        simulationClockReference.clockUpdateEvent += UpdateSimulationClockText;
        simulationClockReference.clockRunningStateChangeEvent += UpdateButtonStates;
        simulationClockReference.clockMultiplierUpdateEvent += UpdateMultiplerButtonStates;
    }

    /// <summary>
    /// Updates the multiplier buttons when a new multiplier update is called from the simulation clock.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UpdateMultiplerButtonStates(object sender, SimulationClock.ClockMultiplerUpdateEventArgs e)
    {
        switch (e.multiplierOld)
        {
            case ClockMultiplier.none:
                ChangeButtonColor(Color.white, x1Button);
                break;
            case ClockMultiplier.two:
                ChangeButtonColor(Color.white, x2Button);
                break;
            case ClockMultiplier.five:
                ChangeButtonColor(Color.white, x5Button);
                break;
            case ClockMultiplier.ten:
                ChangeButtonColor(Color.white, x10Button);
                break;
            case ClockMultiplier.fifteen:
                ChangeButtonColor(Color.white, x15Button);
                break;
            case ClockMultiplier.twenty:
                ChangeButtonColor(Color.white, x20Button);
                break;
        }

        switch (e.multiplierNew)
        {
            case ClockMultiplier.none:
                ChangeButtonColor(Color.cyan, x1Button);
                break;
            case ClockMultiplier.two:
                ChangeButtonColor(Color.cyan, x2Button);
                break;
            case ClockMultiplier.five:
                ChangeButtonColor(Color.cyan, x5Button);
                break;
            case ClockMultiplier.ten:
                ChangeButtonColor(Color.cyan, x10Button);
                break;
            case ClockMultiplier.fifteen:
                ChangeButtonColor(Color.cyan, x15Button);
                break;
            case ClockMultiplier.twenty:
                ChangeButtonColor(Color.cyan, x20Button);
                break;
        }
    }

    /// <summary>
    /// An event listener to the simulation clock running state update. Edits the colour on the buttons.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void UpdateButtonStates(object sender, SimulationClock.ClockRunningStateChangeEventArgs e)
    {
        if (e.clockRunningState == true)
        {
            resumeButton.enabled = false;
            resumeButtonText.text = "Running...";
            ChangeButtonColor(Color.green, resumeButton);
            pauseButton.enabled = true;
            pauseButtonText.text = "Pause";
            ChangeButtonColor(Color.white, pauseButton);
        }
        else
        {
            pauseButton.enabled = false;
            pauseButtonText.text = "Paused...";
            ChangeButtonColor(Color.red, pauseButton);
            resumeButton.enabled = true;
            resumeButtonText.text = "Resume / Start";
            ChangeButtonColor(Color.white, resumeButton);
        }
    }

    /// <summary>
    /// A event listener to the simulation clock update. Edits the text field.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UpdateSimulationClockText(object sender, SimulationClock.ClockUpdateEventArgs e)
    {
        systemClockTMP.text = "Simulation Time: " + e.clockAsString;
    }

    /// <summary>
    /// A helper method which update ALL colors of a button (normal, highlights, selected and pressed).
    /// </summary>
    /// <param name="color">The new color</param>
    /// <param name="b">The button to update</param>
    private void ChangeButtonColor(Color color, Button b)
    {
        ColorBlock cb = b.colors;
        cb.normalColor = color;
        cb.highlightedColor = color;
        cb.pressedColor = color;
        cb.selectedColor = color;
        b.colors = cb;
    }
}
