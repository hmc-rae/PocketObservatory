import getPlanetDetails from '../other/getPlanetDetails';
import * as React from 'react';
import { Text, View, StyleSheet,TouchableOpacity,Image } from 'react-native';
import Constants from 'expo-constants';

const PlanetDetails = ( {id, name} )  => {
    console.log(id, name);
    const details = getPlanetDetails(id).description;
    
    return (
        <View>
            <Text>{name}</Text>
            <Text>{details}</Text>
        </View>
    );
}

export default PlanetDetails