from utils.model_pers import load_model
import sys
import numpy as np

allDataAsString = str(sys.argv[1])
data_path = str(sys.argv[2])


def printType(x):
    print(type(x))


def main():
    svm_clf = load_model(data_path, "knnClf.joblib")
    listOfData = allDataAsString.split(";")

    for dataString in listOfData:
        formattedData = dataString.split(",")
        print(svm_clf.predict([list(map(np.float64, formattedData))]))


if __name__ == "__main__":
    sys.exit(int(main() or 0))
	