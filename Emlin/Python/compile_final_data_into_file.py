import os
import sys
from shutil import copyfile

#dataDirectory = str(sys.argv[1])
dataDirectory = "C:\\Users\\Gerome\\AppData\\Roaming\\Emlin"

listOfFileToAppend = ["D_KeyboardData_O.csv","D_KeyboardData_O_Not.csv"]

groupedDataFilepath = os.path.join(dataDirectory,  "TrainData.csv")
validation_data_filepath = os.path.join(dataDirectory, "ValidationData.csv")
test_data_filepath = os.path.join(dataDirectory, "D_KeyboardData_O_Test.csv")


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
                for line_to_write1 in currentFile:
                    line_to_write2 = line_to_write1.replace('\n', ',' + classifier + '\n')
                    groupedDataFile.write(line_to_write2)

    #with open(validation_data_filepath, 'w+') as validationDataFile:
    #    validationDataFile.write("Id,HT,FT,User\n")
    #    copyfile(dataDirectory + "\\ValidationNotSet\\D_KeyboardData_O_6_Not.csv", validation_data_filepath)

    #    with open(test_data_filepath) as testDataFile:
    #        for line_to_write1 in testDataFile:
    #            line_to_write2 = line_to_write1.replace('\n', ',' + '1' + '\n')
    #            validationDataFile.write(line_to_write2)


if __name__ == "__main__":
    sys.exit(int(main() or 0))
