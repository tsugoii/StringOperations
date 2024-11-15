int n = -1;
if (args.Length < 1)
{
    throw new Exception("No memory size given (first argument should be a number 1-8)");
}
if (!int.TryParse(args[0], out n) || (n < 1 || n > 8))
{
    throw new Exception("Memory size should be a number 1-8");
}

int[]? referenceStringBuffer = null;
int pageFaultCounter = 0;
bool pageFaultStatus = true;
int? victimFrame = null;
List<int?[]> simulated = new();
Queue<int> fifoResolver = new();
int?[] pMemory = new int?[n];

bool looping = true;
while (looping)
{
    MainMenu();
    if (!int.TryParse(Console.ReadLine(), out int userInput))
    {
        Console.WriteLine("Input is Invalid, Please enter a Number");
    }
    else
    {
        OptionSelection(userInput);
    }
}

void MainMenu()
{
    Console.WriteLine("\nPlease Select the Appropriate Menu Option:");
    Console.WriteLine("1\t-\tRead Reference String");
    Console.WriteLine("2\t-\tGenerate Reference String");
    Console.WriteLine("3\t-\tDisplay Current Reference String");
    Console.WriteLine("4\t-\tSimulate First In First Out (FIFO)");
    Console.WriteLine("5\t-\tSimulate Optimal Page Replacement (OPT)");
    Console.WriteLine("6\t-\tSimulate Least Recently Used (LRU)");
    Console.WriteLine("7\t-\tSimulate Least Frequently Used (LFU)");
    Console.WriteLine("0\t-\tExit\n");
    simulated = new();
    pMemory = new int?[n];
    pageFaultCounter = 0;
    pageFaultStatus = true;
    victimFrame = null;
    fifoResolver = new();
}

void OptionSelection(int userInput)
{
    try
    {
        if (userInput == 1)
        {
            ReadReference();
        }
        else if (userInput == 2)
        {
            GenerateReference();
        }
        else if (userInput == 3)
        {
            DisplayReference();
        }
        else if (userInput == 4)
        {
            SimulateFIFO();
        }
        else if (userInput == 5)
        {
            SimulateOPT();
        }
        else if (userInput == 6)
        {
            SimulateLRU();
        }
        else if (userInput == 7)
        {
            SimulateLFU();
        }
        else if (userInput == 0)
        {
            Console.WriteLine("Thank you for utilizing this application!");
            looping = false;
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("Error: Please Enter A Number Between '0 and 7'");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex);
    }
}

void ReadReference()
{
    Console.WriteLine("Please Type a Reference String you would like to utilize");
    Console.WriteLine("This will overwrite any previous Reference String: Type 'QUIT' to go back");
    Console.WriteLine("Enter values separated by spaces, only values 0 - 9 are valid");
    string userReference = Console.ReadLine()!;
    try
    {
        if (userReference.ToUpper() == "QUIT")
        {
            Console.WriteLine("Returning to Main Menu with no changes made");
            return;
        }
        else if (!string.IsNullOrWhiteSpace(userReference))
        {
            parseReferenceString(userReference);
            DisplayReference();
        }
        else
        {
            Console.WriteLine("Please Enter 'QUIT' or any number of values 0-9 separated by spaces");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex);
    }
}

void GenerateReference()
{
    Console.WriteLine("This will Generate a Random Reference String");
    Console.WriteLine("This will overwrite any previous Reference String: Type 'QUIT' to go back");
    Console.WriteLine("Please Enter a Length for the Generated Reference String: ");
    string userMenu = Console.ReadLine()!;
    int userLength = 0;
    var rand = new Random();
    try
    {
        if (userMenu.ToUpper() == "QUIT")
        {
            Console.WriteLine("Returning to Main Menu with no changes made");
            return;
        }
        else if (int.TryParse(userMenu, out userLength))
        {
            referenceStringBuffer = new int[userLength];
            simulated = new();
            Console.WriteLine("Your Reference String is: ");
            for (int i = 0; i < userLength; i++)
                referenceStringBuffer[i] = rand.Next(10);

            DisplayReference();
        }
        else
        {
            Console.WriteLine("Please Enter 'QUIT' or any number of values 0-9 separated by spaces");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex);
    }
}

void DisplayReference()
{
    try
    {
        if (referenceStringBuffer == null)
        {
            throw new Exception("There is no Reference String, complete option 1 or 2 first");
        }
        else
        {
            Console.WriteLine("Your Reference String is: ");
            for (int i = 0; (i + 1) < referenceStringBuffer.Length; i++)
            {
                Console.Write(referenceStringBuffer[i] + " , ");
                if ((i + 2) == referenceStringBuffer.Length)
                {
                    Console.Write(referenceStringBuffer[i + 1]);
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex);
    }
}

void SimulateFIFO()
{
    Console.WriteLine("This will go step-by-step through the FIFO algorithm");
    Console.WriteLine("Press any key to advance to the next step");
    try
    {
        if (referenceStringBuffer == null)
        {
            throw new Exception("There is no Reference String, complete option 1 or 2 first");
        }
        else
        {
            PrintFakeGUI(true);
            int victimFramePosition = 0;

            bool inMemory = false;
            for (int i = 0; i < referenceStringBuffer.Length; i++)
            {//i = reference string position
                inMemory = isInMemory(referenceStringBuffer[i]);

                if (!inMemory) //page not in memory, and memory is full
                {
                    pageFaultStatus = true;
                    pageFaultCounter++;
                    int victim = victimFramePosition % n;
                    victimFramePosition++;
                    victimFrame = pMemory[victim];
                    pMemory[victim] = referenceStringBuffer[i];
                    addToSimluated();
                }
                PrintFakeGUI();
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex);
    }
}

void SimulateOPT()
{
    Console.WriteLine("This will go step-by-step through the OPT algorithm");
    Console.WriteLine("Press any key to advance to the next step");
    try
    {
        if (referenceStringBuffer == null)
        {
            throw new Exception("There is no Reference String, complete option 1 or 2 first");
        }
        else
        {
            PrintFakeGUI(true);
            bool inMemory = false;
            for (int i = 0; i < referenceStringBuffer.Length; i++)
            {//i = reference string position
                inMemory = isInMemory(referenceStringBuffer[i]);

                if (inMemory)
                {
                    if (pageFaultStatus)
                    {
                        enqueuePage(referenceStringBuffer[i]);
                    }
                }
                else //page not in memory, and memory is full
                {
                    pageFaultStatus = true;
                    pageFaultCounter++;

                    //look into future
                    var firstUseDict = new Dictionary<int, int?>();
                    foreach (var p in pMemory)
                    {
                        firstUseDict.Add(p!.Value, null);
                    }
                    for (int j = i + 1; j < referenceStringBuffer.Length; j++)
                    {
                        if (firstUseDict.ContainsKey(referenceStringBuffer[j]))
                        {
                            if (firstUseDict[referenceStringBuffer[j]] == null)
                            {
                                firstUseDict[referenceStringBuffer[j]] = j;
                            }

                            if (firstUseDict.Any(kvp => kvp.Value == null) == false)
                                break;
                        }
                    }

                    List<int> victimOptions = new();
                    int? max = 0;
                    for (int j = 0; j < pMemory.Length; j++)
                    {
                        var opt = firstUseDict[pMemory[j].Value] ?? int.MaxValue;
                        var m = max ?? int.MaxValue;
                        if (opt > m)
                        {
                            max = opt;
                            victimOptions = new() { pMemory[j].Value };
                        }
                        else if (opt == m)
                        {
                            victimOptions.Add(pMemory[j].Value);
                        }
                    }

                    victimFrame = dequeueFirstFrom(victimOptions);
                    int v = Array.IndexOf(pMemory, victimFrame);
                    pMemory[v] = referenceStringBuffer[i];
                    enqueuePage(referenceStringBuffer[i]);
                    addToSimluated();
                }
                PrintFakeGUI();
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex);
    }
}

void SimulateLRU()
{
    Console.WriteLine("This will go step-by-step through the LRU algorithm");
    Console.WriteLine("Press any key to advance to the next step");

    try
    {
        if (referenceStringBuffer == null)
        {
            throw new Exception("There is no Reference String, complete option 1 or 2 first");
        }
        else
        {
            PrintFakeGUI(true);
            bool inMemory = false;
            var seen = new Dictionary<int, int>();
            for (int i = 0; i < referenceStringBuffer.Length; i++)
            {//i = reference string position
                inMemory = isInMemory(referenceStringBuffer[i]);

                if (seen.ContainsKey(referenceStringBuffer[i]))
                {
                    seen[referenceStringBuffer[i]] = i;
                }
                else
                {
                    seen.Add(referenceStringBuffer[i], i);
                }

                if (!inMemory)//page not in memory, and memory is full
                {
                    pageFaultStatus = true;
                    pageFaultCounter++;

                    victimFrame = seen.OrderBy(kvp => kvp.Value).First().Key;

                    int v = Array.IndexOf(pMemory, victimFrame);
                    seen.Remove(pMemory[v].Value);
                    pMemory[v] = referenceStringBuffer[i];
                    addToSimluated();
                }
                PrintFakeGUI();
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex);
    }
}

void SimulateLFU()
{
    Console.WriteLine("This will go step-by-step through the LFU algorithm");
    Console.WriteLine("Press any key to advance to the next step");
    try
    {
        if (referenceStringBuffer == null)
        {
            throw new Exception("There is no Reference String, complete option 1 or 2 first");
        }
        else
        {
            PrintFakeGUI(true);
            bool inMemory = false;
            var count = new Dictionary<int, int>();
            for (int i = 0; i < referenceStringBuffer.Length; i++)
            {//i = reference string position
                inMemory = isInMemory(referenceStringBuffer[i]);

                if (inMemory)
                {
                    if (pageFaultStatus)
                    {
                        enqueuePage(referenceStringBuffer[i]);
                        count.Add(referenceStringBuffer[i], 1);
                    }
                    else
                    {
                        count[referenceStringBuffer[i]]++;
                    }
                }
                else //page not in memory, and memory is full
                {
                    pageFaultStatus = true;
                    pageFaultCounter++;
                    var lowest = count.OrderBy(x => x.Value).First().Value;
                    List<int> victimOptions = count.Where(x => x.Value == lowest).Select(x => x.Key).ToList();

                    victimFrame = dequeueFirstFrom(victimOptions);
                    int v = Array.IndexOf(pMemory, victimFrame);
                    count.Remove(victimFrame.Value);
                    pMemory[v] = referenceStringBuffer[i];
                    count.Add(referenceStringBuffer[i], 1);
                    enqueuePage(referenceStringBuffer[i]);
                    addToSimluated();
                }
                PrintFakeGUI();
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex);
    }
}

bool isInMemory(int? page)
{
    for (int j = 0; j < n; j++)
    {// j = physical memory position
        if (pMemory[j] == page) //already in memory
        {
            pageFaultStatus = false;
            victimFrame = null;
            addToSimluated();
            return true;
        }

        if (pMemory[j] is null) //not in memory and memory is not full
        {
            pMemory[j] = page;
            pageFaultStatus = true;
            pageFaultCounter++;
            victimFrame = null;
            addToSimluated();
            return true;
        }
    }
    return false;
}

void addToSimluated()
{
    simulated.Add(pMemory.ToList().ToArray());
}

void enqueuePage(int page)
{
    //remove anything not in memory from stack
    var queue = new Queue<int>();
    int x;
    while (fifoResolver.Count > 0)
    {
        x = fifoResolver.Dequeue();
        if (pMemory.Contains(x))
        {
            queue.Enqueue(x);
        }
    }

    fifoResolver = queue;

    fifoResolver.Enqueue(page);
}

int dequeueFirstFrom(List<int> options)
{
    int result = -1, x = -1;
    var queue = new Queue<int>();
    while (fifoResolver.Count > 0)
    {
        x = fifoResolver.Dequeue();
        if (!options.Contains(x))
        {
            queue.Enqueue(x);
        }
        else
        {
            result = x;
            break;
        }
    }
    while (fifoResolver.Count > 0)
        queue.Enqueue(fifoResolver.Dequeue());

    fifoResolver = queue;

    return result;
}

void parseReferenceString(string reference)
{
    string[] parts = reference.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    bool invalid = parts.Any(parts => parts.Length > 1) || parts.Any(x => x.Any(y => !char.IsDigit(y)));// all entries are single digits

    if (invalid)
    {
        throw new Exception("Found numbers not separated by spaces/outside of range");
    }

    referenceStringBuffer = reference.Where(char.IsDigit).Select(x => x - '0').ToArray();
    simulated = new();
}

void PrintFakeGUI(bool firstTime = false)
{
    if (firstTime == true)
    {
        Console.WriteLine("Reference String: ");
        for (int j = 0; j < n; j++)
        {
            Console.WriteLine("Physical Frame " + j + " : ");
        }
        Console.WriteLine("Page Faults: ");
        Console.WriteLine("Victim Frames: ");
        firstTime = false;
    }
    else
    {
        Console.WriteLine("Reference String:\t" + string.Join(", ", referenceStringBuffer));
        for (int j = 0; j < n; j++)
        {
            Console.Write("Physical Frame " + j + " :\t");
            for (int i = 0; i < simulated.Count; i++)
            {
                Console.Write(simulated[i][j] is not null ? simulated[i][j].ToString() : " ");
                if (!(i == simulated.Count - 1))
                {
                    Console.Write(", ");
                }
            }
            Console.WriteLine();
        }
        //IMPLEMNT THE COMMA SHIT ABOVE
        Console.WriteLine("Page Faults:\t" + (pageFaultStatus ? "F" : ""));
        Console.WriteLine("Victim Frames:\t" + (victimFrame != null ? victimFrame : ""));
    }
    Console.WriteLine("\nPress a key to continue: ");
    Console.ReadKey();
}