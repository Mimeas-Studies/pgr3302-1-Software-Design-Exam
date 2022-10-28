namespace TextAnalyzer;

public static class FileManager
{
    //  Returns a Queue of words from a filepath
    public static Queue<string> GetText(string filepath)
    {
        var queue = new Queue<string>();

        foreach (string line in File.ReadLines(filepath))
        {
            foreach (var word in line.Split(" "))
            {
                queue.Enqueue(word);
            }
        }

        return queue;
    }
}