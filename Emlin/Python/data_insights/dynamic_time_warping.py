import numpy as np
from scipy.spatial.distance import euclidean

from fastdtw import fastdtw


def get_dtw_value(first_fp, second_fp):
    x = np.loadtxt(first_fp)
    y = np.loadtxt(second_fp)
    distance, path = fastdtw(x, y, dist=euclidean)

    return distance


print(get_dtw_value("ids/ordered_ids_1.txt", "ids/ordered_ids_6.txt"))
print(get_dtw_value("ids/ordered_ids_1.txt", "ids/ordered_ids_10.txt"))
print(get_dtw_value("ids/ordered_ids_6.txt", "ids/ordered_ids_10.txt"))
