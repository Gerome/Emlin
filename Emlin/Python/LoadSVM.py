import ModelUtils as MU
import sys

allDataAsString = str(sys.argv[1])


def main():
    svm_clf = MU.LoadModelFromJoblib("svmClf.joblib")
    listOfData = allDataAsString.split(".")

    for dataString in listOfData:
        formattedData = dataString.split(",")
        print(svm_clf.predict([formattedData]))


if __name__ == "__main__":
    sys.exit(int(main() or 0))