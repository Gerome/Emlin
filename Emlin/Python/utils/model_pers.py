from sklearn.externals import joblib
import os


def save_model(clf, directory, filename):
    joblib.dump(clf, os.path.join(directory, filename, '.joblib'))


def load_model(directory, filename):
    return joblib.load(os.path.join(directory, filename))


