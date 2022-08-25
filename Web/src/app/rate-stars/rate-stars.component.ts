import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-rate-stars',
  templateUrl: './rate-stars.component.html',
  styleUrls: ['./rate-stars.component.css'],
})
export class RateStarsComponent implements OnInit {
  constructor() {}

  ratingStarColors: any[] = [];
  setStarsFlag: boolean = true;
  @Output() newItemEvent = new EventEmitter<string>();
  @Output() setStar: number = 0;
  leaveArrayInput: number[] = [];

  ngOnInit(): void {}

  addNewItem(value: number) {
    this.newItemEvent.emit(value.toString());
  }

  // prettier-ignore
  adjustHoverColor(ratingStarIndex: number): void{    
    const activate = "color: #fbc634 !important";
    const disable = "";
    switch(ratingStarIndex){
      case(1): this.changeTheRatingStarArray([0], activate); break;
      case(2): this.changeTheRatingStarArray([0, 1], activate); break;
      case(3): this.changeTheRatingStarArray([0, 1, 2], activate); break;
      case(4): this.changeTheRatingStarArray([0, 1, 2, 3], activate); break;
      case(5): this.changeTheRatingStarArray([0, 1, 2, 3, 4], activate); break;
      
      case(-1): if(this.checkIfSet(1)){this.changeTheRatingStarArray([0], disable); break;} else {break;}
      case(-2): if(this.checkIfSet(2)){this.changeTheRatingStarArray([0, 1], disable); break;} else {break;}
      case(-3): if(this.checkIfSet(3)){this.changeTheRatingStarArray([0, 1, 2], disable); break;} else {break;}
      case(-4): if(this.checkIfSet(4)){this.changeTheRatingStarArray([0, 1, 2, 3], disable); break;} else {break;}
      case(-5): if(this.checkIfSet(5)){this.changeTheRatingStarArray([0, 1, 2, 3, 4], disable); break;} else {break;}
      }
    }

  setStarColor(clickedStar: number) {
    const activate = 'color: #fbc634 !important';
    const disable = '';
    this.setStarsFlag = false;
    this.setStar = clickedStar;
    this.addNewItem(this.setStar);

    for (var i = 0; i < 5; i++) {
      this.ratingStarColors[i] = disable;
    }

    for (var i = 0; i < clickedStar; i++) {
      this.ratingStarColors[i] = activate;
    }
  }

  private checkIfSet(starIndex: number): boolean {
    if (starIndex >= this.setStar) {
      return true;
    } else return false;
  }

  private changeTheRatingStarArray(ratingStars: number[], change: string) {
    //var myRatingStars = ratingStars.splice(0, this.setStar-1);
    ratingStars.forEach((item) => {
      (this.ratingStarColors[ratingStars[item]] = change), false;
    });
  }
}
