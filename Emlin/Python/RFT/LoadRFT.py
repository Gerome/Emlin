from Helper import ModelUtils as MU
import sys
import numpy as np
allDataAsString = str(sys.argv[1])


def printType(x):
    print(type(x))


def main():
    rft_clf = MU.LoadModelFromJoblib("rftClf.joblib")
    listOfData = allDataAsString.split(";")

    for dataString in listOfData:
        formattedData = dataString.split(",")
        print(rft_clf.predict([list(map(np.float64, formattedData))]))


if __name__ == "__main__":
    sys.exit(int(main() or 0))