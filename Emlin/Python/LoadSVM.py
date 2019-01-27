import ModelSaveLoad as MSL
import SVMModel
import sys

data = str(sys.argv[1])

def main():
    svm_clf = MSL.LoadModelFromJoblib("svmClf.joblib")
    result = data.split(",")
    print(result)
    print(svm_clf.predict([result]))

if __name__ == "__main__":
    sys.exit(int(main() or 0))