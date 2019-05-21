import numpy as np
from scipy.spatial.distance import euclidean
from fastdtw import fastdtw


def get_dtw_value(first_fp, second_fp):
    x = np.loadtxt(first_fp)
    y = np.loadtxt(second_fp)
    distance, path = fastdtw(x, y, dist=euclidean)

    return distance


user1 = "ids/ordered_ids_1.txt"
user2 = "ids/ordered_ids_2.txt"
user3 = "ids/ordered_ids_3.txt"
user4 = "ids/ordered_ids_4.txt"
user5 = "ids/ordered_ids_5.txt"
user6 = "ids/ordered_ids_6.txt"

print(get_dtw_value(user1, user2))
print(get_dtw_value(user1, user3))
print(get_dtw_value(user1, user4))
print(get_dtw_value(user1, user5))
print(get_dtw_value(user1, user6))

print(get_dtw_value(user2, user3))
print(get_dtw_value(user2, user4))
print(get_dtw_value(user2, user5))
print(get_dtw_value(user2, user6))

print(get_dtw_value(user3, user4))
print(get_dtw_value(user3, user5))
print(get_dtw_value(user3, user6))

print(get_dtw_value(user4, user5))
print(get_dtw_value(user4, user6))

print(get_dtw_value(user5, user6))
