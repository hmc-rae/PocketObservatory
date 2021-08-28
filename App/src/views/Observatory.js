import React, {useState, useEffect} from 'react';
import { View, Text, StyleSheet} from 'react-native';
import { Camera } from 'expo-camera';

export default function Observatory(){
  const [hasPermission, setHasPermission] = useState(null);
  const type = Camera.Constants.Type.back;

  useEffect(() => {
    (async () => {
      const { status } = await Camera.requestPermissionsAsync();
      setHasPermission(status === 'granted');
    })();
  }, []);

  if (hasPermission === null) {
    return <View />;
  }
  if (hasPermission === false) {
    return <Text>No access to camera</Text>;
  }
  return (
    <View style = {styles.container}>
      <Camera style = {styles.camera} type={type}/>
    </View>
    );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  camera: {
    flex: 1,
  },
});