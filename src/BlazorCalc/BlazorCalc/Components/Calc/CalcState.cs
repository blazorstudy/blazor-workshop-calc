using System.Xml;

namespace BlazorCalc.Components.Calc;

public class CalcState
{
    enum CalcOp
    {
        None,
        Plus,
        Minus,
    }

    private CalcOp op = CalcOp.None;

    public event Action? Notify; 
    public int DisplayValue { get; set; } = 0;
    public int StoredValue { get; set; } = 0;

    public void Clear()
    {
        DisplayValue = 0;
        StoredValue = 0;
        op = CalcOp.None;
    }

    public void clickButton(string value)
    {
        switch (value)
        {
            case "0":
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
                DisplayValue = DisplayValue * 10 + int.Parse(value); 
                break;

            case "+":
                op = CalcOp.Plus;
                StoredValue = DisplayValue;
                DisplayValue = 0;
                break;

            case "-":
                op = CalcOp.Minus;
                StoredValue = DisplayValue;
                DisplayValue = 0;
                break;

            case "=":
                if (op == CalcOp.Plus)
                {
                    DisplayValue += StoredValue;
                }
                else if (op == CalcOp.Minus)
                {
                    DisplayValue = StoredValue - DisplayValue;
                }
                StoredValue = 0;
                op = CalcOp.None;
                break;
            case "C":
                Clear();
                break;
        }

        writeStatus();
        Notify?.Invoke();
    }

    private void writeStatus()
    {
        Console.WriteLine($"DisplayValue[{this.DisplayValue}] StoredValue[{this.StoredValue}] op[{this.op}]");
    }
}
