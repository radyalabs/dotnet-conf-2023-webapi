using System.Text;

namespace dotnetConf2023.UnitTests.Validators.DataTests;

public class CorrectDataTestStringIsBase64 : IEnumerable<object[]>
{
    public CorrectDataTestStringIsBase64()
    {
        Data = new List<object[]>();

        const string s = "Test@1234";
        var s1 = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
        Data.Add(new object[] { s1, true });
        Data.Add(new object[] { "Test1", false });
        Data.Add(new object[] { "Abe232", false });
    }

    private List<object[]> Data { get; }

    public IEnumerator<object[]> GetEnumerator() => Data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}