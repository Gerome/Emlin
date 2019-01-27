from sklearn.externals import joblib
import os

def SaveModelAsJoblib(clf, filename):
	joblib.dump(clf, os.path.dirname(os.path.abspath(__file__)) + "\\" +filename + '.joblib')

def LoadModelFromJoblib(filename):
	return joblib.load(os.path.dirname(os.path.abspath(__file__)) + "\\" + filename)