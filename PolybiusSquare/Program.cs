using System.Text;

#region Setup
const string Space_Position = "99";
Console.OutputEncoding = Encoding.UTF8; // Add appropriate encoding for the cyrilic alphabet
#endregion

char[,] alphabet = new char[,]
{
    {'а', 'б', 'в', 'г', 'д', 'е'},
    {'ж', 'з', 'и', 'й', 'к', 'л'},
    {'м', 'н', 'о', 'п', 'р', 'с'},
    {'т', 'у', 'ф', 'х', 'ц', 'ч'},
    {'ш', 'щ', 'ъ', 'ь', 'ю', 'я'},
    {'-','-','-','-','-','-' },
};


var cipher = Encrypt(alphabet, "информационна сигурност");
Console.WriteLine($"Cipher: {cipher}");

var plainText = Decrypt(alphabet, cipher);
Console.WriteLine($"Plain text: {plainText}");

static string Encrypt(char[,] alphabet, string text)
{
    StringBuilder sb = new StringBuilder();

    foreach (var character in text)
    {
        var characterPosition = FindCharacterEncryptedPosition(alphabet, character);
        sb.Append(characterPosition);
    }

    return sb.ToString();
}

static string Decrypt(char[,] alphabet, string cipher)
{
    var doubles = string
        .Join(string.Empty,
              cipher.Select((x, i) => i > 0 && i % 2 == 0 ? $" {x}" : x.ToString()))
        .Split(' ')
        .ToList();

    StringBuilder sb = new StringBuilder();

    foreach (string position in doubles)
    {
        var letter = DecryptLetter(alphabet, position);
        sb.Append(letter);
    }

    return sb.ToString();
}

static string FindCharacterEncryptedPosition(char[,] alphabet, char character)
{
    for (int i = 0; i < alphabet.GetLength(0); i++)
    {
        for (int j = 0; j < alphabet.GetLength((1)); j++)
        {
            if (character == alphabet[i, j])
            {
                return $"{i + 1}{j + 1}";
            }
        }
    }

    return Space_Position;
}

static string DecryptLetter(char[,] alphabet, string position)
{
    if (position == Space_Position)
    {
        return " ";
    }

    var indexes = position.ToCharArray();
    int row = (indexes[0] - '0') - 1;
    int column = (indexes[1] - '0') - 1;

    return alphabet[row, column].ToString();
}