import ModelUtils as MU
import SVMModel
import sys
import numpy as np
allDataAsString = str(sys.argv[1])

def printType(x):
	print(type(x))

def main():
    svm_clf = MU.LoadModelFromJoblib("knnClf.joblib")
    listOfData = allDataAsString.split(".")

    for dataString in listOfData:
        formattedData =  dataString.split(",")
        print(svm_clf.predict([list(map(np.float64, formattedData))]))


if __name__ == "__main__":
    sys.exit(int(main() or 0))