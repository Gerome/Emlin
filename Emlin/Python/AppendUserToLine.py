import os
import sys

dataDirectory = str(sys.argv[1])

listOfFileToAppend = ["D_KeyboardData_O.csv","D_KeyboardData_O_Not.csv"]

groupedDataFilepath = os.path.join(dataDirectory,"ProcessedData.csv")


def main():
    with open(groupedDataFilepath, 'w+') as groupedDataFile:
        groupedDataFile.write("Id,HT,FT,User\n")

    for file in listOfFileToAppend:

        file_path = os.path.join(dataDirectory, file)

        if file_path.__contains__("Not"):
            classifier = "0"
        else:
            classifier = "1"

        with open(file_path) as currentFile:
            with open(groupedDataFilepath, 'a') as groupedDataFile:
                for lineToWrite1 in currentFile:
                    lineToWrite2 = lineToWrite1.replace('\n', ',' + classifier + '\n')
                    groupedDataFile.write(lineToWrite2)


if __name__ == "__main__":
    sys.exit(int(main() or 0))
