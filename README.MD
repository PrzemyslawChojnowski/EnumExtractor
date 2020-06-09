1. Clone repo (git clone)
2. Open with VS and restore packages.
3. In VS right click Enum.g4 -> Run custom tool
4. Build project
5. Run exe file from command line with arguments: 
first: directory with cs files to parse (relative to exe directory)
second: directory for output (also relative)
e.g. (bash) `./EnumExtractor.exe i o`

e.g. C# input: 

public class ColorTypes
{
	Red = 1,
	Blue = 2,
	Black = 3,
	Green = 4
}

equivalent JS output:

const colorTypesDict = {
	red: 1,
	blue: 2,
	black: 3,
	green: 4
};

export default colorTypesDict;