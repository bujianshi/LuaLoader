LOCAL_PATH := $(call my-dir)
 
###################################<mainproc>###################################
include $(CLEAR_VARS)

LOCAL_MODULE		:= injector
LOCAL_ARM_MODE		:= arm
LOCAL_CPP_EXTENSION	:= .cpp
LOCAL_C_INCLUDES	:= $(LOCAL_PATH)/include/
LOCAL_CFLAGS	+= -fvisibility=hidden -pie -fPIE
LOCAL_LDLIBS	+= -L$(SYSROOT)/usr/lib -llog
LOCAL_LDFLAGS 	+= -pie -fPIE
LOCAL_SRC_FILES	:= \
				injector\android-injector.cpp \
				injector\injector.cpp

include $(BUILD_EXECUTABLE)
############################### libsubstrate #################################
include $(CLEAR_VARS)

LOCAL_MODULE := substrate
LOCAL_SRC_FILES := libsubstrate.so
LOCAL_CFLAGS += -fvisibility=hidden

include $(PREBUILT_SHARED_LIBRARY)
############################### libevent #################################
include $(CLEAR_VARS)
LOCAL_MODULE	:= event
LOCAL_SRC_FILES := libevent.a
include $(PREBUILT_STATIC_LIBRARY) 
###################################<mainso>###################################

include $(CLEAR_VARS)

LOCAL_MODULE		:= xgame
LOCAL_STATIC_LIBRARIES	+= event
LOCAL_ARM_MODE		:= arm
LOCAL_LDLIBS		+=	-L$(SYSROOT)/usr/lib -llog 
LOCAL_C_INCLUDES	:= $(LOCAL_PATH)/include/
LOCAL_CPP_EXTENSION	:=	.cpp
LOCAL_CPPFLAGS		+= -fexceptions

LOCAL_SRC_FILES		:= \
					json\json_writer.cpp \
					json\json_reader.cpp \
					json\json_value.cpp \
					core\substrate-hook.cpp \
					core\common-help.cpp \
					core\lua-hotload.cpp
					

include $(BUILD_SHARED_LIBRARY)
############# install ##############
include $(CLEAR_VARS)

dest_path	:= /data/local/tmp

all:
	
	adb push $(NDK_APP_DST_DIR)/injector $(dest_path)
	adb push $(NDK_APP_DST_DIR)/libxgame.so $(dest_path)
	adb push $(NDK_APP_DST_DIR)/libsubstrate.so $(dest_path)
	adb shell "su -c 'chmod 777 $(dest_path)/*'"