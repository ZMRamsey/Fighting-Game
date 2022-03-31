using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public enum FlexibleMode
    {
        Default,
        Width,
        Height,
        FixedRows,
        FixedColumns
    }
    [SerializeField] int _rows;
    [SerializeField] int _columns;
    [SerializeField] Vector2 _cellSize;
    [SerializeField] Vector2 _spacing;
    [SerializeField] FlexibleMode _mode;
    [SerializeField] bool _fitX, _fitY;
    
    public override void CalculateLayoutInputVertical() {

    }

    public override void SetLayoutHorizontal() {
        base.CalculateLayoutInputHorizontal();

        if (_mode == FlexibleMode.Width || _mode == FlexibleMode.Height || _mode == FlexibleMode.Default) {
            _fitX = true;
            _fitY = true;

            float squareRoot = Mathf.Sqrt(transform.childCount);
            _rows = Mathf.CeilToInt(squareRoot);
            _columns = Mathf.CeilToInt(squareRoot);
        }

            if (_mode == FlexibleMode.Width || _mode == FlexibleMode.FixedColumns) {
                _rows = Mathf.CeilToInt(transform.childCount / (float)_columns);
            }
            if (_mode == FlexibleMode.Height || _mode == FlexibleMode.FixedRows) {
                _columns = Mathf.CeilToInt(transform.childCount / (float)_rows);
            }
        

        var myWidth = rectTransform.rect.width;
        var myHeight = rectTransform.rect.height;

        Vector2 spacingCalc = Vector2.zero;
        spacingCalc.x = ((_spacing.x / (float)_columns) * (_columns - 1));
        spacingCalc.y = ((_spacing.y / (float)_rows) * (_rows - 1));

        Vector2 paddingCalc = Vector2.zero;
        paddingCalc.x = (padding.left / (float)_columns) + (padding.right / (float)_columns);
        paddingCalc.y = (padding.top / (float)_rows) + (padding.bottom / (float)_rows);

        var cellWidth = (myWidth / (float)_columns) - spacingCalc.x - paddingCalc.x;
        var cellHeight = (myHeight / (float)_rows) - spacingCalc.y - paddingCalc.y;

        _cellSize.x = _fitX ? cellWidth : _cellSize.x;
        _cellSize.y = _fitY ? cellHeight : _cellSize.y;

        var columnCount = 0;
        var rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++) {
            rowCount = i / _columns;
            columnCount = i % _columns;

            var child = rectChildren[i];

            var xPos = (_cellSize.x * columnCount) + (_spacing.x * columnCount) + padding.left;
            var yPos = (_cellSize.y * rowCount) +(_spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(child, 0, xPos, _cellSize.x);
            SetChildAlongAxis(child, 1, yPos, _cellSize.y);
        }
    }

    public override void SetLayoutVertical() {

    }
}
