import { Component, Input } from "@angular/core";
import { TrainModelResult } from "../../models/models";

@Component({
  selector: 'app-trainer-result',
  styleUrls: ['./trainer-result.component.css'],
  templateUrl: './trainer-result.component.html',
})
export class TrainerResultComponent {
  @Input() trainModelResult: TrainModelResult = {};
}
