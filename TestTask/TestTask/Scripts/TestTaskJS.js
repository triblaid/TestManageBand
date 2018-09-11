function CreatePoints() {
	var countPoints = document.getElementById("countPoints").value;

	var data = {
		countPoints: countPoints
	};

	$.ajax({
		type: "POST",
		url: "/Home/CreatePoints",
		data: data,
		success: function (viewhtml) {
			$('#PointsContainer').html(viewhtml);
		},
		error: function () {
			console.log("Error. Cant get this page");
		}
	});
}

function AnalysisPolygon() {
	var vertex = [];
	var PointsContainer = document.getElementById("PointsContainer");
	for (var i = 0, n = 0; i < (PointsContainer.children.length - 1); i++ , n += 2) {
		vertex[n] = PointsContainer.children[i].children[1].value;
		vertex[n + 1] = PointsContainer.children[i].children[2].value;
	}

	var data = {
		Points: vertex,
		CPX: document.getElementById("checkPointX").value,
		CPY: document.getElementById("checkPointY").value
	}

	$.ajax({
		type: "POST",
		url: "/Home/AnalysisPolygon",
		contentType: 'application/json',
		dataType: 'json',
		data: JSON.stringify(data),
		success: function (result) {
			if (result.IsSuccess) {
				DrawPolygon();
				alert(result.Message);
			}
			else {
				alert("Произошла ошибка. " + result.Message);
			}
		},
		error: function (result) {
			alert("Сервер не отвечает");
		}
	});
}

function DrawPolygon() {
	var PointsContainer = document.getElementById("PointsContainer");
	var myGridObject = {
		canvasWidth: 400, //ширина холста
		canvasHeight: 400, //высота холста
		cellsNumberX: 20, //количество ячеек по горизонтали
		cellsNumberY: 20, //количество ячеек по вертикали
		color: "#00BFFF", //цвет линий
		//Метод setSettings устанавливает все настройки
		setSettings: function () {
			// получаем наш холст по id
			canvas = document.getElementById("mycanvas");
			// устанавливаем ширину холста
			canvas.width = this.canvasWidth;
			// устанавливаем высоту холста
			canvas.height = this.canvasHeight;
			// canvas.getContext("2d") создает объект для рисования
			ctx = canvas.getContext("2d");
			// задаём цвет линий
			ctx.beginPath();
			ctx.lineWidth = 4;
			//собираем координаты точек
			for (var i = 0, n = 0; i < (PointsContainer.children.length - 1); i++ , n += 2) {
				ctx.lineTo(PointsContainer.children[i].children[1].value, PointsContainer.children[i].children[2].value);
			}
			//замыкаем
			ctx.lineTo(PointsContainer.children[0].children[1].value, PointsContainer.children[0].children[2].value);
			ctx.fillStyle = "#f00";//Красим нашу точку
			ctx.fillRect(document.getElementById("checkPointX").value - 3, document.getElementById("checkPointY").value - 3, 6, 6);
			ctx.strokeStyle = "#000"; //красим полигон
			ctx.font = "italic 16pt Arial";
			ctx.strokeText("0,0", 0, 16);
			ctx.stroke(); // отрисоываем
			ctx.strokeStyle = this.color;
			ctx.lineWidth = 1;
			ctx.globalAlpha = 0.2;//прозрачность
			// вычисляем ширину ячейки по горизонтали
			lineX = canvas.width / this.cellsNumberX;
			// вычисляем высоту ячейки по вертикали
			lineY = canvas.height / this.cellsNumberY;
		},

		// данная функция как раз и будет отрисовывать сетку
		drawGrid: function () {
			// в переменной buf будет храниться начальная координата, откуда нужно рисовать линию
			// с каждой итерацией она должна увеличиваться либо на ширину ячейки, либо на высоту
			var buf = 0;
			// Рисуем вертикальные линии
			for (var i = 0; i <= this.cellsNumberX; i++) {
				// начинаем рисовать
				ctx.beginPath();
				// ставим начальную точку
				ctx.moveTo(buf, 0);
				// указываем конечную точку для линии
				ctx.lineTo(buf, canvas.height);
				// рисуем и выводим линию
				ctx.stroke();
				buf += lineX;
			}
			buf = 0;
			// Рисуем горизонтальные линии
			for (var j = 0; j <= this.cellsNumberY; j++) {
				ctx.beginPath();
				ctx.moveTo(0, buf);
				ctx.lineTo(canvas.width, buf);
				ctx.stroke();
				buf += lineY;
			}
		}
	}
	myGridObject.setSettings();
	myGridObject.drawGrid();
}