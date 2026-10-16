using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Menu navigation with rows and columns.
public class Navigation : MonoBehaviour {

    public event EventHandler<EventArgs> OnNavigate; 
    public event EventHandler<EventArgs> OnDefaultInteract;

    [Serializable]
    public class NavigationRow {
        public List<Highlight> columns;
    }

    [SerializeField] List<NavigationRow> rows;
    [SerializeField] GameObject nextPage;
    
    private const float DELAY = 1f;
    private bool blockInteract = true;
    private int row, column;
    private Highlight currentHighlight;

    private void Start() {
        // hide all highlights
        foreach (var row in rows) {
            foreach (var button in row.columns) {
                button.Hide();
            }
        }
        
        // select and show first highlight
        row = column = 0;
        currentHighlight = rows[row].columns[column];
        currentHighlight.Show();

        StartCoroutine(DelayInteract());
    }

    // Prevent interaction for short period to avoid accidental clicks on a sudden transition.
    private IEnumerator DelayInteract() {
        yield return new WaitForSeconds(DELAY);
        blockInteract = false;
    }

    private void OnDown() {
        if (ChangeRow(1))
            Focus();
    }

    private void OnUp() {
        if (ChangeRow(-1))
            Focus();
    }

    private void OnLeft() {
        if (ChangeColumn(-1))
            Focus();
    }

    private void OnRight() {
        if (ChangeColumn(1))
            Focus();
    }

    // Move up or down a row. Wraps around and finds the closest column.
    private bool ChangeRow(int change) {
        int previousRow = row;
        int rowCount = rows.Count;
        row = (row + change) % rowCount;
        if (row < 0) row += rowCount; 

        int newRowCols = rows[row].columns.Count;

        int prevRowCols = rows[(row - change + rowCount) % rowCount].columns.Count;
        float columnRatio = (float) column / Mathf.Max(prevRowCols - 1, 1);
        column = Mathf.RoundToInt(columnRatio * (newRowCols - 1));
        column = Mathf.Clamp(column, 0, newRowCols - 1);

        return previousRow != row;
    }

    // Move left or right to new column.
    private bool ChangeColumn(int change) {
        int previousColumn = column;
        int columnCount = rows[row].columns.Count;
        column = (column + change) % columnCount;
        if (column < 0) column += columnCount; 
        return previousColumn != column;
    }

    // Highlight a new button.
    private void Focus() {
        if (currentHighlight != null)
            currentHighlight.Hide();
        
        currentHighlight = rows[row].columns[column];
        currentHighlight.Show();
        OnNavigate?.Invoke(this, EventArgs.Empty);
    }

    // Interact with selected button.
    private void OnInteract() {
        if (blockInteract) return;
        rows[row].columns[column].GetComponent<Interactable>().RunInteract();
        OnDefaultInteract?.Invoke(this, EventArgs.Empty);

        // if next page is set, immediately move to next page after interaction
        if (nextPage != null) {
            nextPage.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    // Returns currently selected object.
    public GameObject GetSelection() {
        return rows[row].columns[column].gameObject;
    }
}
