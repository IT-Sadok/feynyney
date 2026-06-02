string message = "passworder";

char[] characters = message.ToCharArray();
string result = string.Empty;

for (int i = 0; i < characters.Length - 4; i++)
{
    characters[i] = '#';
}

for (int i = 0; i < characters.Length; i++)
{
    result += characters[i];
}

Console.WriteLine(result);
    
