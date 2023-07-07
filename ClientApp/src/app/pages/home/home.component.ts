import { Component, OnInit } from '@angular/core';
import { TrainModelResult } from '../../models/models';
import { MLService } from '../../services/MLService';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent{
  isLoading: boolean = false;
  isModelTrained: boolean = false;
  prediction: string = "";
  trainModelResult: TrainModelResult = {};

  constructor(private mLService: MLService) { }
 
  train() {
    this.isLoading = true;
    this.mLService.train().then(res => {
      this.isLoading = false;
      this.isModelTrained = true;
      this.trainModelResult = res;
    }); 
  }

  predict() {
    this.isLoading = true;
    this.mLService.predict().then(res => {
      this.isLoading = false;
      this.prediction = res.prediction;
    })
  }
}
