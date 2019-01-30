from sklearn.externals import joblib
from sklearn.model_selection import cross_val_score
import os

def SaveModelAsJoblib(clf, filename):
	joblib.dump(clf, os.path.dirname(os.path.abspath(__file__)) + "\\" +filename + '.joblib')

def LoadModelFromJoblib(filename):
	return joblib.load(os.path.dirname(os.path.abspath(__file__)) + "\\" + filename)


def GetScoreFromCLF(clf, all_x, all_y):
	return cross_val_score(clf, all_x, all_y, cv=5, verbose=True, n_jobs=5)
